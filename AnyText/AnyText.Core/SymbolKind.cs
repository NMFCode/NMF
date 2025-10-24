namespace NMF.AnyText
{
    /// <summary>
    /// Symbol kinds used for document symbol requests
    /// </summary>
    public enum SymbolKind
    {
        /// <summary>
        /// the element should be displayed like a file
        /// </summary>
        File = 1,
        /// <summary>
        /// the element should be displayed like a module
        /// </summary>
        Module = 2,
        /// <summary>
        /// the element should be displayed like a namespace
        /// </summary>
        Namespace = 3,
        /// <summary>
        /// the element should be displayed like a package
        /// </summary>
        Package = 4,
        /// <summary>
        /// the element should be displayed like a class
        /// </summary>
        Class = 5,
        /// <summary>
        /// the element should be displayed like a method
        /// </summary>
        Method = 6,
        /// <summary>
        /// the element should be displayed like a property
        /// </summary>
        Property = 7,
        /// <summary>
        /// the element should be displayed like a field
        /// </summary>
        Field = 8,
        /// <summary>
        /// the element should be displayed like a constructor
        /// </summary>
        Constructor = 9,
        /// <summary>
        /// the element should be displayed like an enumeration
        /// </summary>
        Enum = 10,
        /// <summary>
        /// the element should be displayed like an interface
        /// </summary>
        Interface = 11,
        /// <summary>
        /// the element should be displayed like a function
        /// </summary>
        Function = 12,
        /// <summary>
        /// the element should be displayed like a variable
        /// </summary>
        Variable = 13,
        /// <summary>
        /// the element should be displayed like a constant
        /// </summary>
        Constant = 14,
        /// <summary>
        /// the element should be displayed like a string
        /// </summary>
        String = 15,
        /// <summary>
        /// the element should be displayed like a number
        /// </summary>
        Number = 16,
        /// <summary>
        /// the element should be displayed like a boolean
        /// </summary>
        Boolean = 17,
        /// <summary>
        /// the element should be displayed like an array
        /// </summary>
        Array = 18,
        /// <summary>
        /// the element should be displayed like an object
        /// </summary>
        Object = 19,
        /// <summary>
        /// the element should be displayed like a key
        /// </summary>
        Key = 20,
        /// <summary>
        /// the element should be displayed like ´the null constant
        /// </summary>
        Null = 21,
        /// <summary>
        /// the element should be displayed like an enumeration literal
        /// </summary>
        EnumMember = 22,
        /// <summary>
        /// the element should be displayed like a structure
        /// </summary>
        Struct = 23,
        /// <summary>
        /// the element should be displayed like an event
        /// </summary>
        Event = 24,
        /// <summary>
        /// the element should be displayed like an operator
        /// </summary>
        Operator = 25,
        /// <summary>
        /// the element should be displayed like a type parameter
        /// </summary>
        TypeParameter = 26,
    }
}
