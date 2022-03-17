module App

open Elmish
open Elmish.React
open Feliz
open Feliz.Bulma

// import Sass file
Fable.Core.JsInterop.importAll "./style.scss"

type Model =
    { Bill: float
      TipPercentage: float
      NumberOfPeople: int }

let init () =
    { Bill = 0.0
      TipPercentage = 0.15
      NumberOfPeople = 1 }

let update msg model = model

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
                            prop.placeholder $"{model.Bill}"
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
                Bulma.field.div [
                    field.isGrouped
                    prop.children [
                        Bulma.control.div [
                            Bulma.button.a [
                                color.hasBackgroundDark
                                color.hasTextWhite
                                prop.text "5%"
                            ]
                        ]
                        Bulma.control.div [
                            Bulma.button.a [
                                color.hasBackgroundDark
                                color.hasTextWhite
                                prop.text "10%"
                            ]
                        ]
                        Bulma.control.div [
                            Bulma.button.a [
                                // is selected
                                color.hasBackgroundDark
                                color.hasTextWhite
                                prop.text "15%"
                            ]
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
                            prop.placeholder $"{model.NumberOfPeople}"
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

let renderRight model dispatch =
    Bulma.container [
        color.hasBackgroundDark
        prop.children [
            Html.text "Hello Right!"
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
