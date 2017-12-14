PageOfBob.Parsing.Compiled
==========================

An experimental, tiny library for writing tiny parsers in C#.  Use simple rules to define complex parsers and then generate the appropriate IL at runtime for those parsers.

An example CSV parser:

```
using System.Collections.Generic;
using static PageOfBob.Parsing.Compiled.Rules;

var quote = Match('"');
var comma = Match(',');
var whitespace = Match(' ', '\t').Many(false);
var eol = Match('\r', '\n').Many().Required();

var doubleQuote = quote.ThenIgnore(quote);
var quotedContent = Any(quote.Not(), doubleQuote).ManyAsText();

var quotedValue = quote.ThenKeep(quotedContent).ThenIgnore(quote);
var unquotedValue = Match('"', ',', '\n', '\r').Not().ManyAsText();
var value = Any(quotedValue, unquotedValue).ThenIgnore(whitespace);
var secondValue = comma.ThenIgnore(whitespace).ThenKeep(value);

var line = value
    .Then(secondValue.Many(), (first, list) =>
    {
        list.Insert(0, first);
        return list;
    })
    .ThenIgnore(eol);

// Parses a single line of CSV-encoded content
// Use AsEnumerable extension method to parse multiple lines.
return line.CompileParser("CsvLineParser");
```

---

This library is based off PageOfBob.Parsing, but with a focus on performance.

TODO:

* More Testing
* Documenation
* Possiblity to generate DLL with parser pre-built?
