Oracle.ManagedDataAccess.Core NuGet Package 2.19.180 README
===========================================================
Release Notes: Oracle Data Provider for .NET Core

January 2023

This provider supports .NET Core 3.1 and .NET 6.

This document provides information that supplements the Oracle Data Provider for .NET (ODP.NET) documentation.

You have downloaded Oracle Data Provider for .NET. The license agreement is available here:
https://www.oracle.com/downloads/licenses/distribution-license.html

New Feature
===========
- SEPS (Secure External Password Store) Support
Oracle Data Provider for .NET Core now supports SEPS on non-Windows platforms.
  

Bug Fixes since Oracle.ManagedDataAccess.Core NuGet Package 2.19.170
====================================================================
Bug 31456063 ORA-03111 IS ENCOUNTERED WHILE CANCELLING THE CURRENT COMMAND EXECUTION
Bug 31793997 INCORRECT UDTS ARE RETURNED BY DATAREADER AFTER A NULL UDT IS FETCHED 
Bug 34431232 CURRENT DATABASE EDITION NAME IS NOT RETURNED BY ORACLECONNECTION
Bug 34617083 RESOLVE CVE-2023-21893

Known Issues and Limitations
============================
1) BindToDirectory throws NullReferenceException on Linux when LdapConnection AuthType is Anonymous

https://github.com/dotnet/runtime/issues/61683

This issue is observed when using System.DirectoryServices.Protocols, version 6.0.0.
To workaround the issue, use System.DirectoryServices.Protocols, version 5.0.1.

 Copyright (c) 2021, 2023, Oracle and/or its affiliates. 
