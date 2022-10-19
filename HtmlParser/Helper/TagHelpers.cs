namespace HtmlParser.Helper
{
    public class HdrezkaTagHelpers
    {
        public string TableTagHelper { get; set; } = @"//table[@class='b-post__info']";
        public string ImageSourceTagHelper { get; set; } = @"//div[@class='b-sidecover']/a";
        public string NameRuTagHelper { get; set; } = @"//div[@class='b-post__title']";
        public string OriginalNameTagHelper { get; set; } = @"//div[@class='b-post__origtitle']";
        public string LinksTagHelper { get; set; } = @"//div/a";
        public string PlayerTagHelper { get; set; } = @"//div[@class='b-player']";
    }
}
