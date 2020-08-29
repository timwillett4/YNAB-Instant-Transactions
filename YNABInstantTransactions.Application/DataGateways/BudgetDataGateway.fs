module YNABInstantTransactions.DataGateways.BudgetDataGateway

open YNABInstantTransactions

type BudgetDataGateway =
    abstract member LoadBudget : BudgetID : uint -> Budget

