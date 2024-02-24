namespace WebshopTemplate.Extensions;

// Extension method to set the basket in session
public static class SessionExtensions
{
    public static void SetBasket(this ISession session, string key, Basket basket)
    {
        session.SetString(key, JsonConvert.SerializeObject(basket));
    }

    public static Basket? GetBasket(this ISession session, string key)
    {
        var basketString = session.GetString(key);
        return basketString == null ? null : JsonConvert.DeserializeObject<Basket>(basketString);
    }
}