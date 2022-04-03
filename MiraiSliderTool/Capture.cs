using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiSliderTool
{

    public class CustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame,
IRequest request, IResponse response)
        {

            if (request.Url.IndexOf("cap_union_new_verify") != -1)
            {
                var filter = FilterManager.CreateFilter(request.Identifier.ToString());
                return filter;
            }
            else {
                return null;
            }

            
        }

        protected override void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            IResponse response, UrlRequestStatus status, long receivedContentLength)
        {

            
            //Trace.WriteLine(request.Url);
            if (request.Url.IndexOf("cap_union_new_verify") != -1)
            {
                var filter = FilterManager.GetFileter(request.Identifier.ToString()) as CustomFilter;
                if (filter != null)
                {
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string data = encoding.GetString(filter.DataAll.ToArray());
                    BrowserForm.showTicket(data);
                }
            }
        }
    }

    public class CustomFilter : IResponseFilter
    {
        public List<byte> DataAll = new List<byte>();

        public FilterStatus Filter(System.IO.Stream dataIn, out long dataInRead, System.IO.Stream dataOut, out long dataOutWritten)
        {
            try
            {
                if (dataIn == null)
                {
                    dataInRead = 0;
                    dataOutWritten = 0;

                    return FilterStatus.Done;
                }
                if (dataIn.Length > dataOut.Length)
                {
                    dataInRead = dataIn.Length;
                    dataOutWritten = dataOut.Length;
                    return FilterStatus.Done;
                }

                dataInRead = dataIn.Length;
                dataOutWritten = Math.Min(dataInRead, dataOut.Length);

                dataIn.CopyTo(dataOut);
                dataIn.Seek(0, SeekOrigin.Begin);
                byte[] bs = new byte[dataIn.Length];
                dataIn.Read(bs, 0, bs.Length);
                DataAll.AddRange(bs);

                dataInRead = dataIn.Length;
                dataOutWritten = dataIn.Length;

                return FilterStatus.NeedMoreData;
            }
            catch (Exception ex)
            {
                dataInRead = dataIn.Length;
                dataOutWritten = dataIn.Length;
                //throw ex;
                return FilterStatus.Done;
            }
        }

        public bool InitFilter()
        {
            return true;
        }

        public void Dispose()
        {

        }
    }

    public class FilterManager
    {
        private static Dictionary<string, IResponseFilter> dataList = new Dictionary<string, IResponseFilter>();

        public static IResponseFilter CreateFilter(string guid)
        {
            lock (dataList)
            {
                var filter = new CustomFilter();
                dataList.Add(guid, filter);

                return filter;
            }
        }

        public static IResponseFilter GetFileter(string guid)
        {
            lock (dataList)
            {
                if (dataList.ContainsKey(guid))
                {
                    return dataList[guid];
                }
                else
                {
                    return null;
                }

            }
        }
    }

    public class CustomRequestHandler : CefSharp.Handler.RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new CustomResourceRequestHandler();
        }
    }

}
