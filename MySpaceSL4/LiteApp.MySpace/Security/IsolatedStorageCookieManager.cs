//using System.IO;
//using System.IO.IsolatedStorage;

//namespace LiteApp.MySpace.Security
//{
//    public class IsolatedStorageCookieManager : ICookieManager
//    {
//        public void SaveUserInfo(UserInfoCookie userInfo)
//        {
//            try
//            {
//                var originalInfo = ReadUserInfo();
//                if (originalInfo != null)
//                    originalInfo.LastSignedInUser = userInfo.LastSignedInUser;
//                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
//                {
//                    using (var stream = store.OpenFile("userinfo", FileMode.OpenOrCreate))
//                    {
//                        using (var writer = new StreamWriter(stream))
//                        {
//                            if (originalInfo != null)
//                            {
//                                writer.Write(originalInfo.ToJson());
//                            }
//                            else
//                            {
//                                writer.Write(userInfo.ToJson());
//                            }
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                // Do nothing
//            }
//        }

//        public UserInfoCookie ReadUserInfo()
//        {
//            try
//            {
//                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
//                {
//                    using (var stream = store.OpenFile("userinfo", FileMode.Open))
//                    {
//                        using (var reader = new StreamReader(stream))
//                        {
//                            var json = reader.ReadToEnd();
//                            return UserInfoCookie.FromJson(json);
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                return null;
//            }
//        }
//    }
//}
