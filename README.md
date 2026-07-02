i refactored the code quite a bit but one thing to note is when the app is running the potential commands to be used have changed from solution that was originally attached so for example 

getting the server list is still the same so - partycli.exe server_list

But for getting country servers it won't be like this - partycli.exe server_list --france
but like this - partycli.exe server_list --country france 
and this can be changed to 1 of 3 countries so france, albania and argentina 

Same with getting servers by protocol so instead of this - partycli.exe server_list --TCP
it'll be this - partycli.exe server_list --protocol TCP
and likewise this can be changed to 1 of 3 protocols so UDP, TCP and NordLynx


Unit tests are bit more iffy, i ended up using .NET 8 for the test project and the tests might fail on first run. This down to the architectural differences between both projects but if you change the Processor Architecture for AnyCPU Projects in the Test drop down on visual studio from Auto to X86 then rebuild the solution the tests will run (Completely ineffcient but for the purposes of this it should be fine)
