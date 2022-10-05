using NUnit.Framework;

namespace HttpWrapper.Test;

internal class HttpGetterShould
{
    [Test]
    public async Task Get_Data_Using_Basic_Authentication()
    {
        const string address = "http://httpbin.org";
        const string requestUri = "/basic-auth/foo/bar";
        const string username = "foo";
        const string password = "bar";

        var httpGetter = new HttpGetter();


        var result = await httpGetter.GetWithBasicAuthentication(address, requestUri, username, password);


        const string expected = "{\n  \"authenticated\": true, \n  \"user\": \"foo\"\n}\n";
        Assert.That(result, Is.EqualTo(expected));
    }
}
