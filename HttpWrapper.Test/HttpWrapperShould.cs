using NUnit.Framework;

namespace HttpWrapper.Test;

internal class HttpWrapperShould
{
    [Test]
    public async Task Get_Data_Using_Basic_Authentication()
    {
        const string address = "http://httpbin.org";
        const string requestUri = "/basic-auth/foo/bar";
        const string username = "foo";
        const string password = "bar";

        var httpWrapper = new HttpWrapper();


        var result = await httpWrapper.GetWithBasicAuthentication(address, requestUri, username, password);


        const string expected = "{\n  \"authenticated\": true, \n  \"user\": \"foo\"\n}\n";
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task Post_Data_Using_Basic_Authentication()
    {
        const string address = "http://httpbin.org";
        const string requestUri = "/post";
        const string body = "abc 123";

        var httpWrapper = new HttpWrapper();


        var result = await httpWrapper.Post(address, requestUri, body);


        Assert.That(result, Does.Contain("\"data\": \"abc 123\", "));
    }
}
