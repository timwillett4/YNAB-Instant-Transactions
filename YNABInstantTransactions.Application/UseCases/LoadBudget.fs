module YNABInstantTransactions.UseCases.LoadBudget

open YNABInstantTransactions.DataGateways.BudgetDataGateway

let execute (budgetGateway:BudgetDataGateway) budgetID =
   
    budgetID |> budgetGateway.LoadBudget
    
