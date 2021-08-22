using ComicsLibrary.Common.Data;

namespace ComicsLibrary.Blazor
{
    public static class ShelfExtensionMethods
    {
        public static string GetName(this Shelf shelf)
        {
            return shelf switch
            {
                Shelf.ToReadNext => "To Read Next",
                Shelf.PutAside => "Put Aside",
                _ => shelf.ToString()
            };
        }
    }
}
