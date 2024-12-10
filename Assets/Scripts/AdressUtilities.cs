using System.Text.RegularExpressions;

public class AdressUtilities
{
    public static Address ConvertHtmlToAddress(string html)
    {
        Address address = new Address();

        address.StreetAddress = ExtractTagContent(html, "street-address");
        address.PostalCode = ExtractTagContent(html, "postal-code");
        address.Locality = ExtractTagContent(html, "locality");
        address.Region = ExtractTagContent(html, "region");
        address.Country = ExtractTagContent(html, "country-name");

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
}