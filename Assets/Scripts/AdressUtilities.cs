using System.Text.RegularExpressions;

public class AdressUtilities
{
    public static Address ConvertHtmlToAddress(string html)
    {
        Address address = new Address(
            ExtractTagContent(html, "street-address"),
            ExtractTagContent(html, "postal-code"),
            ExtractTagContent(html, "locality"),
            ExtractTagContent(html, "region"),
            ExtractTagContent(html, "country-name")
            );

        return address;
    }

    private static string ExtractTagContent(string html, string tagName)
    {
        var match = Regex.Match(html, $@"<span class=""{tagName}"">(.*?)</span>");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    public static string ConvertAddressToHtml(Address address)
    {
        return $@"
    <span class=""street-address"">{address.StreetAddress}</span>, 
    <span class=""postal-code"">{address.PostalCode}</span> 
    <span class=""locality"">{address.Locality}</span>/<span class=""region"">{address.Region}</span>, 
    <span class=""country-name"">{address.Country}</span>";
    }
    
}

[System.Serializable]
public class Address
{
    public string StreetAddress;
    public string PostalCode;
    public string Locality;
    public string Region;
    public string Country;

    public Address(string streetAddress, string postalCode, string locality, string region, string country)
    {
        StreetAddress = streetAddress;
        PostalCode = postalCode;
        Locality = locality;
        Region = region;
        Country = country;
    }
}