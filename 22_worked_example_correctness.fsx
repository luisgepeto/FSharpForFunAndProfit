//a shopping cart can be empty, active or paid for
//an empty cart becomes active when adding an item
//removing last item of a cart becomes empty
//paying an active cart becomes paidfor

//items can only be added ot empty or active carts
//items can only be removed from active carts
//carts can only be payed if active


type CartItem = string

type EmptyState = NoItems
type ActiveState = { UnpaidItems: CartItem list; }
type PaidForState = { PaidItems : CartItem list; 
                      Payment: decimal}

type Cart =
    | Empty of EmptyState
    | Active of ActiveState
    | PaidFor of PaidForState

//we proceed to create operations for each state
let addToEmptyState item = Cart.Active { UnpaidItems = [item]}

let addToActiveState state itemToAdd =
    let newList = itemToAdd :: state.UnpaidItems
    Cart.Active { state with UnpaidItems = newList}

let removeFromActiveState state itemToRemove = 
    let newList = state.UnpaidItems |> List.filter (fun i-> i<>itemToRemove)
    match newList with
    | [] -> Cart.Empty NoItems
    | _ -> Cart.Active { state with UnpaidItems = newList} 

let payForActiveState state amount = 
    Cart.PaidFor { PaidItems = state.UnpaidItems; Payment = amount }

// we can attach operations to states as methods
type EmptyState with
    member this.Add = addToEmptyState
type ActiveState with
    member this.Add = addToActiveState this
    member this.Remove = removeFromActiveState this
    member this.Pay = payForActiveState this

// we can add cart level helper methods
let addItemToCart cart item =
    match cart with
    | Empty state -> state.Add item
    | Active state-> state.Add item
    | PaidFor _ -> 
        printfn "ERROR: The cart is paid for"
        cart

let removeItemToCart cart item = 
    match cart with
    | Empty _ ->
        printfn "ERROR: The cart is empty"
        cart
    | Active state -> state.Remove item
    | PaidFor _-> 
        printfn "ERROR: The cart is paid for"
        cart

let payCart cart amount = 
    match cart with
    | Empty _ ->
        printfn "ERROR: The cart is empty"
        cart
    | Active state -> state.Pay amount
    | PaidFor _ -> 
        printfn "ERROR: The cart is paid for"
        cart

let displayCart cart =
    match cart with
    | Empty _ -> printfn "The cart is empty"
    | Active state -> printfn "The cart contains %A unpaid items" state.UnpaidItems
    | PaidFor state -> printfn "The cart contains %A paid items. Amount paid: %f" state.PaidItems state.Payment

type Cart with
    static member NewCart = Cart.Empty NoItems
    member this.Add = addItemToCart this
    member this.Remove = removeItemToCart this
    member this.Pay = payCart this
    member this.Display = displayCart this
        


