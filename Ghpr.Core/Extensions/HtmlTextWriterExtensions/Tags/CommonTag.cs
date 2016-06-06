using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Ghpr.Core.Extensions.HtmlTextWriterExtensions.Tags
{
    public static class CommonTag
    {
        public static void AddTag(this HtmlTextWriter writer, HtmlTextWriterTag tag,
            Dictionary<string, string> attributes, string value = "")
        {
            foreach (var attribute in attributes)
            {
                writer.AddAttribute(attribute.Key, attribute.Value);
            }
            writer.RenderBeginTag(tag);
            if (value != "")
            {
                writer.Write(value);
            }
            writer.RenderEndTag();
        }

        public static HtmlTextWriter ForEach<T>(this HtmlTextWriter writer, List<T> objects, Action<T> action)
        {
            foreach (var obj in objects)
            {
                action.Invoke(obj);
            }
            return writer;
        }

        public static HtmlTextWriter ForEach<T>(this HtmlTextWriter writer, T[] objects, Action<T> action)
        {
            foreach (var obj in objects)
            {
                action.Invoke(obj);
            }
            return writer;
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag,
            Dictionary<HtmlTextWriterAttribute, string> attributes, string value = "")
        {
            foreach (var attribute in attributes)
            {
                writer.AddAttribute(attribute.Key, attribute.Value);
            }
            writer.RenderBeginTag(tag);
            if (value != "")
            {
                writer.Write(value);
            }
            writer.RenderEndTag();
            return writer;
        }

        public static void AddTag(this HtmlTextWriter writer, HtmlTextWriterTag tag,
            Dictionary<string, string> attributes1,
            Dictionary<HtmlTextWriterAttribute, string> attributes2,
            string value = "")
        {
            foreach (var attribute in attributes1)
            {
                writer.AddAttribute(attribute.Key, attribute.Value);
            }
            foreach (var attribute in attributes2)
            {
                writer.AddAttribute(attribute.Key, attribute.Value);
            }
            writer.RenderBeginTag(tag);
            if (value != "")
            {
                writer.Write(value);
            }
            writer.RenderEndTag();
        }

        public static void AddTag(this HtmlTextWriter writer, HtmlTextWriterTag tag,
            string value = "")
        {
            writer.RenderBeginTag(tag);
            writer.Write(value != "" ? value : Environment.NewLine);
            writer.RenderEndTag();
        }

        public static void AddTag(this HtmlTextWriter writer, string tag, string value = "")
        {
            writer.RenderBeginTag(tag);

            writer.Write(value != "" ? value : Environment.NewLine);
            writer.RenderEndTag();
        }
        
        public static HtmlTextWriter OpenTag(this HtmlTextWriter writer, HtmlTextWriterTag tag)
        {
            writer.RenderBeginTag(tag);
            return writer;
        }

        public static HtmlTextWriter OpenTag(this HtmlTextWriter writer, string tag)
        {
            writer.RenderBeginTag(tag);
            return writer;
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, string tag, string value)
        {
            return writer.OpenTag(tag).Text(value).CloseTag();
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, string tag)
        {
            return writer.OpenTag(tag).CloseTag();
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag)
        {
            return writer.OpenTag(tag).CloseTag();
        }

        public static HtmlTextWriter CloseTag(this HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            return writer;
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag, Action someAction)
        {
            writer.OpenTag(tag);
            someAction.Invoke();
            return writer.CloseTag();
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, string tag, Action someAction)
        {
            writer.OpenTag(tag);
            someAction.Invoke();
            return writer.CloseTag();
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag, Action<HtmlTextWriter> someAction)
        {
            writer.OpenTag(tag);
            someAction.Invoke(writer);
            return writer.CloseTag();
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag, string value)
        {
            return writer.OpenTag(tag).Text(value).CloseTag();
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag,
            Dictionary<string, string> attributes, string value = "")
        {
            foreach (var attribute in attributes)
            {
                writer.AddAttribute(attribute.Key, attribute.Value);
            }
            writer.RenderBeginTag(tag);
            if (value != "")
            {
                writer.Write(value);
            }
            writer.RenderEndTag();
            return writer;
        }

        public static HtmlTextWriter DoAction(this HtmlTextWriter writer, Action someAction)
        {
            someAction.Invoke();
            return writer;
        }

        public static HtmlTextWriter If(this HtmlTextWriter writer, bool condition, Action someAction)
        {
            return condition ? writer.DoAction(someAction) : writer;
        }

        public static HtmlTextWriter TagIf(this HtmlTextWriter writer, bool condition, HtmlTextWriterTag tag)
        {
            return condition ? writer.Tag(tag) : writer;
        }

        public static HtmlTextWriter TagIf(this HtmlTextWriter writer, bool condition, HtmlTextWriterTag tag, string value)
        {
            return condition ? writer.Tag(tag, value) : writer;
        }

        public static HtmlTextWriter TagIf(this HtmlTextWriter writer, bool condition, HtmlTextWriterTag tag, Action someAction)
        {
            return condition ? writer.Tag(tag, someAction) : writer;
        }
    }
}
