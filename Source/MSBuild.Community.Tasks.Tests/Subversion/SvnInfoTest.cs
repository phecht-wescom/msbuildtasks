using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using MSBuild.Community.Tasks.Subversion;
using NUnit.Framework;


namespace MSBuild.Community.Tasks.Tests.Subversion
{
    [TestFixture]
    public class SvnInfoTests
    {
        private string ProjectDir
        {
            get
            {
                Assembly assembly = Assembly.GetCallingAssembly();

                FileInfo finfo = new FileInfo(assembly.FullName);

                return finfo.Directory.Parent.Parent.FullName;
            }
        }

        /// <summary>
        /// This test is coupled with the repository. It may require network connectivity
        /// in order to pass (haven't yet tested). Also, if this site is ever rehosted
        /// such that the URL changes then this test will need to be updated. The upside here
        /// is that we actually run "svn info" and test some if it's results.
        /// </summary>
        [Test]
        public void TestInfoReturnValues()
        {
            SvnInfo info = new SvnInfo();
            info.LocalPath = ProjectDir;

            info.BuildEngine = new MockBuild();
            info.Execute();

            string val = info.RepositoryPath;

            // "http://msbuildtasks.tigris.org/svn/msbuildtasks/trunk"
            // could also be svn://
            Assert.AreEqual(0, val.IndexOf("http://"));
            Assert.Greater(val.IndexOf("Source/MSBuild.Community.Tasks.Tests"), 0);
            Assert.AreEqual(NodeKind.directory, info.NodeKind);
            Assert.AreEqual("http://msbuildtasks.tigris.org/svn/msbuildtasks", info.RepositoryRoot);
            Assert.AreNotEqual(Guid.Empty, info.RepositoryUuid);
        }
    }
}