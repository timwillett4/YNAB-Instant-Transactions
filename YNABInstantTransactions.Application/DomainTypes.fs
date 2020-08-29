[<AutoOpen>]
module YNABInstantTransactions.DomainTypes

open System

type Transaction = 
    { ID : Guid
      Account : string
      Date : DateTime // @TODO - use YODA time package to deal with timezones
      Payee : string
      Amount : decimal }

type Category =
    { ID : Guid
      Name : string }

type Budget = 
    { ID : Guid
      Name : string
      Categories : Category list }

