// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("vZM95e94xttC2PrlmthpPT9tsSkTqrXUM08loFdufzrosdC+QMGh1yHFP+ztaIxVndYOZb5QRKt867thMYmcmEOUB3kdWtkCWf7erLIVD+zJtjT1kz3LTnzEyQRKN4V+kS7Oa3jKSWp4RU5BYs4Azr9FSUlJTUhLy4KXVb7XpxaxcP8Yat1Hscl2UHJJx1JwPiMUfTT8g/WFyZNag+RfWa6GE6PS3lCEzxE28qjmcnZ5tAF9lEWAG1VjSXjFFyUFsJyjp92+z8d9GkPoymBSk9LonivrxsZRNh/Bw8GJ68tluo4UPCP5VAa0eiaQezdKyklHSHjKSUJKyklJSO8QxW1++hMdIUG8/bUao6O0PHr3p0OH6LNR1XSCpmWBARObz0pLSUhJ");
        private static int[] order = new int[] { 9,11,2,6,4,9,7,10,12,13,13,13,13,13,14 };
        private static int key = 72;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
