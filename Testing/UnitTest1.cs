using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logger;
using System.IO;
namespace Testing
{
    [TestClass]
    public class UnitTestFile//test file
    {
        ToFile file = new ToFile();

        [TestMethod]
        public void TestMethodExists()
        {
            Assert.AreEqual(file.Exists(), File.Exists("log.txt"));
            file.Send("");
            Assert.IsTrue(file.Exists());
            Assert.AreNotEqual(file.Exists(), File.Exists("qwe.txt"));
        }
        [TestMethod]
        public void TestMethodSend()//запись в файл
        {
            string s = "1";
            Assert.IsTrue(file.Send(s));
            s += "2";
            Assert.IsTrue(file.Send(s));
        }
        [TestMethod]
        public void TestMethodRead()//считывание с файла
        {
            string s = "3";
            file.Send(s);
            Assert.IsNotNull(file.GetFromFile());
            Assert.IsNull(file.GetFromFile());
        }
    }
    [TestClass]
    public class UnitTestMail//test mail
    {
        static MailInfo info = new MailInfo();
        ToMail mail = new ToMail(info);
        [TestMethod]
        public void TestMethodSend()
        {
            string s = "1234567890";
            Assert.IsTrue(mail.Send(s));
            info.Pass = "1234567890";
            mail = new ToMail(info);
            Assert.IsFalse(mail.Send(s));
        }
    }
}
