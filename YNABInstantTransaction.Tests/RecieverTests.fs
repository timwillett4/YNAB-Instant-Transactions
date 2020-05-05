module YBANInstantTransaction.Appliation.RecieverTests

open System
open Expecto
open YNABInstantTransaction.Application.Reciever

type TestCase = {
    TestName : string
    TestMessage : string
    SMS : string    
    ExpectedResult : Transaction option
}

// @TODO - add more samples
// @TODO - check with YNAB how to add payee (should I include city/state)
// include time and timezone in transaction?
let MFCUSamples = 
    [{TestName = "2 digit charge from MFCU"
      TestMessage = "2 digit Transaction SMS should return Some Transaction"
      SMS= "Charge for $14.99 on 04/25 09:13 CDT at ZOOM.US, SAN JOSE, CA on card ending in 5124."
      ExpectedResult = Some <| {Account="5124"; Date=new DateTime(2020, 4, 25); Payee="ZOOM.US"; Amount=14.99M}}

     {TestName = "Single digit charge from MFCU"
      TestMessage = "Sing digit transaction SMS should return Some Transaction"
      SMS= "Charge for $0.35 on 04/04 13:38 CDT at GLACIER WATER VENDING, WINSTON SALEM, NC on card ending in 5124."
      ExpectedResult = Some <| {Account="5124"; Date=new DateTime(2020, 4, 4); Payee="GLACIER WATER VENDING"; Amount=0.35M}}

     {TestName = "4 digit charge from MFCU"
      TestMessage = "4 digit transaction SMS should return Some Transaction"
      SMS= "Charge for $1900.00 on 03/09 10:18 CDT at INSTACART, SAN FRANCISCO, CA on card ending in 5124."
      ExpectedResult = Some <| {Account="5124"; Date=new DateTime(2020, 3, 9); Payee="INSTACART"; Amount=1900.00M}}

     {TestName = "International charge from MFCU"
      TestMessage = "International Transaction SMS should return Some Transaction"
      SMS= "Charge for $4.00 on 04/22 15:33 CDT at BOARDGAMEARENA, +33617258034, CA on card ending in 5124."
      ExpectedResult = Some <| {Account="5124"; Date=new DateTime(2020, 4, 22); Payee="BOARDGAMEARENA"; Amount=4.00M}}]

let NonTransactionSMSs = 
    [{TestName = "NonTransaction.IncludesDollar"
      TestMessage = "Regular SMS with a currency in message should return None"
      SMS= "$207 to my mom"
      ExpectedResult=None}] 

[<Tests>]
let tests =
  let createSMSParseTest testCase = test testName { 
    let actual = testCase.SMS |> parseSMS
    Expect.equal actual testCase.ExpectedResult testCase.TestMessage } |> testLabel testCase.TestName
  testList "SMS Parse Tests" (MFCUSamples @ NonTransactionSMSs |> List.map createSMSParseTest)