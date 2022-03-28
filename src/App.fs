module App

open System
open Elmish
open Elmish.React
open Feliz
open Feliz.Bulma

// import Sass file
Fable.Core.JsInterop.importAll "./style.scss"

type TipPercentage =
    | Five
    | Ten
    | Fifteen
    | TwentyFive
    | Fifty
    | Custom

type Model =
    { Bill : float
      TipPercentage : TipPercentage
      CustomTipPercentage : float
      NumberOfPeople : int }

type Msg =
    | SelectTipPercentage of TipPercentage
    | SetBillAmount of float
    | SetNumberOfPeople of int
    | Reset

let init () =
    { Bill = 142.55
      TipPercentage = Fifteen
      CustomTipPercentage = 0.0
      NumberOfPeople = 5 }

let inline max x y = if x < y then y else x

let inline min x y = if x < y then x else y

let update msg model =
    match msg with
    | SelectTipPercentage tipPercentage -> { model with TipPercentage = tipPercentage }
    | SetBillAmount bill -> { model with Bill = max bill 0.0 }

    | SetNumberOfPeople num -> { model with NumberOfPeople = max num 1 }

    | Reset -> init ()

let tipSelections =
    [ "5%", Five
      "10%", Ten
      "15%", Fifteen
      "25%", TwentyFive
      "50%", Fifty
      "Custom", Custom ]

let renderTipSelections model dispatch (percentage : TipPercentage) (text : string) =
    Bulma.tab [
        if percentage = model.TipPercentage then
            tab.isActive
        prop.children [
            Bulma.button.a [
                if percentage = model.TipPercentage then
                    color.hasBackgroundPrimary
                    color.hasTextPrimaryDark
                else
                    color.hasBackgroundDark
                    color.hasTextWhite
                prop.text text
                prop.onClick (fun _ -> dispatch <| SelectTipPercentage percentage)
            ]
        ]
    ]

let renderLeft model dispatch =
    Bulma.container [
        prop.children [
            // Bill Amount
            Bulma.field.div [
                Bulma.label "Bill"
                Bulma.control.div [
                    control.hasIconsLeft
                    prop.children [
                        Bulma.input.number [
                            text.hasTextRight
                            prop.valueOrDefault model.Bill
                            prop.onChange (SetBillAmount >> dispatch)
                        ]
                        Html.span [
                            Bulma.icon [
                                icon.isLeft
                                prop.classes [ "fas"; "fa-dollar-sign" ]
                            ]
                        ]
                    ]
                ]
            ]
            // Select Tip %
            Bulma.field.div [
                Bulma.label "Select Tip %"
                Bulma.tabs [
                    tabs.isToggle
                    prop.children [
                        Html.ul [
                            for (txt, case) in tipSelections do
                                renderTipSelections model dispatch case txt
                        ]
                    ]
                ]
            ]
            // Number of People
            Bulma.field.div [
                Bulma.label "Number of People"
                Bulma.control.div [
                    control.hasIconsLeft
                    prop.children [
                        Bulma.input.number [
                            text.hasTextRight
                            prop.valueOrDefault $"{model.NumberOfPeople}"
                            prop.onChange (SetNumberOfPeople >> dispatch)
                        ]
                        Bulma.icon [
                            icon.isLeft
                            prop.classes [ "fas"; "fa-user" ]
                        ]
                    ]
                ]
            ]
        ]
    ]

let renderTipAmountAndTotal (model : Model) =
    let tipPercent =
        match model.TipPercentage with
        | Five -> 0.05
        | Ten -> 0.10
        | Fifteen -> 0.15
        | TwentyFive -> 0.25
        | Fifty -> 0.50
        | Custom -> model.CustomTipPercentage

    let tipAmount = model.Bill * tipPercent

    let tipAmountPerPerson = tipAmount / (float model.NumberOfPeople)

    let total = model.Bill + tipAmount

    let totalPerPerson = total / (float model.NumberOfPeople)

    Html.div [
        Html.div [
            Html.text "Tip Amount"
            Html.text "/ person"
            Html.output $"$%0.2f{tipAmountPerPerson}"
        ]
        Html.div [
            Html.text "Total"
            Html.text "/ person"
            Html.output $"$%0.2f{totalPerPerson}"
        ]
    ]

let renderResetButton model dispatch =
    Bulma.button.reset [
        prop.onClick (fun _ -> dispatch Reset)
    ]

let renderRight model dispatch =
    Bulma.container [
        color.hasBackgroundDark
        prop.children [
            Html.text "Hello Right!"
            renderTipAmountAndTotal model
            renderResetButton model dispatch
        ]
    ]

let renderLevel model dispatch =
    Bulma.tile [
        tile.isAncestor
        color.hasBackgroundWhite
        // text.hasTextCentered
        prop.children [
            Bulma.tile [
                tile.isParent
                // tile.is5
                prop.children [
                    Bulma.tile [
                        tile.isChild
                        prop.children [
                            renderLeft model dispatch
                        ]
                    ]
                ]
            ]
            Bulma.tile [
                tile.isParent
                // tile.is5
                prop.children [
                    Bulma.tile [
                        tile.isChild
                        prop.children [
                            renderRight model dispatch
                        ]
                    ]
                ]
            ]
        ]
    ]

let renderTitle model dispatch =
    Bulma.title [
        color.isDark
        text.hasTextCentered
        text.isUppercase
        prop.text "splitter"
    ]

let render model dispatch =
    React.fragment [
        renderTitle model dispatch
        renderLevel model dispatch
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "fable-app"
|> Program.run
