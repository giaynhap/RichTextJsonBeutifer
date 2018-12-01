using CallApiInvoice.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallApiInvoice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string endpoint = this.txEndpoint.Text;
            string contentType = this.txContentType.Text;
            string request = this.txRequest.Text;
            string method = this.txMethod.Text;

           string response = Request.webRequest(endpoint, request, method,contentType);
            this.txResponse.Text = response;
        }

      

        private void txRequest_MouseUp(object sender, MouseEventArgs e)
        {

            RichTextBox res = sender as RichTextBox;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   //click event
                //MessageBox.Show("you got it!");
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Tag = res;
                menuItem.Click += new EventHandler(CutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Tag = res;
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Tag = res;
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.MenuItems.Add(menuItem);

                menuItem = new MenuItem("Beautifier");
                menuItem.Tag = res;
                menuItem.Click += new EventHandler(Beautifier);
                contextMenu.MenuItems.Add(menuItem);

                menuItem = new MenuItem("Minifier");
                menuItem.Tag = res;
                menuItem.Click += new EventHandler(Minifier);
                contextMenu.MenuItems.Add(menuItem);

                res.ContextMenu = contextMenu;
            }
        }
        void CutAction(object sender, EventArgs e)
        {
            MenuItem res = sender as MenuItem;
            RichTextBox elm = res.Tag as RichTextBox;
            elm.Cut();
        }

        void CopyAction(object sender, EventArgs e)
        {
            MenuItem res = sender as MenuItem;
            RichTextBox elm = res.Tag as RichTextBox;
            Graphics objGraphics;
            Clipboard.SetData(DataFormats.Rtf, elm.SelectedRtf);
        ///    Clipboard.Clear();
        }

        void Beautifier(object sender, EventArgs e)
        {
            MenuItem res = sender as MenuItem;
            RichTextBox elm = res.Tag as RichTextBox;
            try
            {
               var js = StringHelpers.beutifer(elm.Text);
                var text = js;
                text = SyntaxHighlightJson(text);
                text = text.Replace("\r\n", @"\line");
                text = text.Replace("{", @"\{");
                text = text.Replace("}", @"\}");
                elm.Rtf = @"{\rtf1\ansi\deff0 {\colortbl;\red0\green0\blue0;\red100\green100\blue100;\red108\green44\blue140;\red234\green163\blue32;\red35\green66\blue142;\red102\green11\blue32;}" + text + @"}";
            }
            catch { }
        }
        void Minifier(object sender, EventArgs e)
        {
            MenuItem res = sender as MenuItem;
            RichTextBox elm = res.Tag as RichTextBox;
            try
            {
                var js = StringHelpers.minifier(elm.Text);
                elm.Text = js;
            }
            catch { }
        }


        public string SyntaxHighlightJson(string original)
        {
          
            return Regex.Replace(
              original,
              @"(¤(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\¤])*¤(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)".Replace('¤', '"'),
              match => {
                  var cls = "number";
                  if (Regex.IsMatch(match.Value, @"^¤".Replace('¤', '"')))
                  {
                      if (Regex.IsMatch(match.Value, ":$"))
                      {
                          cls = "key";
                      }
                      else
                      {
                          cls = "string";
                      }
                  }
                  else if (Regex.IsMatch(match.Value, "true|false"))
                  {
                      cls = "boolean";
                  }
                  else if (Regex.IsMatch(match.Value, "null"))
                  {
                      cls = "null";
                  }
                  if (cls == "null") {
                      return @"\cf2 null \cf0";
                  }
                  else if (cls=="number")
                  {
                      return @"\cf3 "+ match + @" \cf0";
                  }
                  else if (cls == "string")
                  {
                      return @"\cf4 " + StringHelpers.GetRtfUnicodeEscapedString(match.ToString()) + @" \cf0";
                  }
                  else if (cls == "key")
                  {
                      return @"\cf5 " + match + @" \cf0";
                  }
                  else if (cls == "boolean")
                  {
                      return @"\cf6 " + match + @" \cf0";
                  }
                  else
                      return @"\cf2 "+ match + @" \cf0"; ;
              });
        }


        void PasteAction(object sender, EventArgs e)
        {
            MenuItem res = sender as MenuItem;
            RichTextBox elm = res.Tag as RichTextBox;
            if (Clipboard.ContainsText(TextDataFormat.Rtf))
            {
                elm.SelectedRtf
                    = Clipboard.GetData(DataFormats.Rtf).ToString();
            }
        }

        private void txResponse_MouseUp(object sender, MouseEventArgs e)
        {
            txRequest_MouseUp(sender, e);
        }
    }
}
