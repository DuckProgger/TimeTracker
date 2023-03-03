using UI.Infrastructure;

namespace UI.Tests;

public class DateTimeUtilsTest
{
    [Theory]
    [InlineData("10:10", "10:10:00")]
    [InlineData("1010", "10:10:00")]
    [InlineData("23:59", "23:59:00")]
    [InlineData("2359", "23:59:00")]
    [InlineData("0:20", "00:20:00")]
    [InlineData("020", "00:20:00")]
    [InlineData("01:10", "01:10:00")]
    [InlineData("0110", "01:10:00")]
    [InlineData("110", "01:10:00")]
    [InlineData("01:4", "01:04:00")]
    [InlineData("1:1", "01:01:00")]
    [InlineData("00:00", "00:00:00")]
    [InlineData("0:00", "00:00:00")]
    [InlineData("00:0", "00:00:00")]
    [InlineData("000", "00:00:00")]
    [InlineData("0000", "00:00:00")]
    [InlineData("1515", "15:15:00")]
    [InlineData("0515", "05:15:00")]
    [InlineData("800", "08:00:00")]
    [InlineData("10", "00:10:00")]
    [InlineData("5", "00:05:00")]
    [InlineData("0", "00:00:00")]
    public void Can_parse_timespan_string(string timeString, string expected)
    {
        var parseSuccess = DateTimeUtils.TryParse(timeString, out var parsedTimeSpan);

        var parsedDateTimeString = parsedTimeSpan.ToString();
        Assert.Equal(expected, parsedDateTimeString);
        Assert.True(parseSuccess);
    }

    [Theory]
    [InlineData("24:00")]
    [InlineData(":20")]
    [InlineData("01:")]
    [InlineData("01:77")]
    [InlineData(":")]
    [InlineData("1:1:")]
    [InlineData(":1:")]
    [InlineData("999")]
    [InlineData("00000")]
    [InlineData("2515")]
    [InlineData("1080")]
    [InlineData("80")]
    [InlineData("170")]
    [InlineData("0575")]
    [InlineData("1975")]
    [InlineData("975")]
    [InlineData("a")]
    public void Can_not_parse_timespan_string(string timeString)
    {
        var expected = TimeSpan.Zero;

        var parseSuccess = DateTimeUtils.TryParse(timeString, out var parsedDateTimeString);

        Assert.Equal(expected, parsedDateTimeString);
        Assert.False(parseSuccess);
    }
}