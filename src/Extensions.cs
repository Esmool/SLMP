namespace SLMP {
    public static class Extensions {
        public static int[] AsIntArray(this ushort[] data) {
            int[] result = new int[data.Length];

            for (int i = 0; i < data.Length; i++) {
                result[i] = Convert.ToInt32(data[i]);
            }
            
            return result;
        }

        public static byte[] AsByteArray(this ushort[] data) {
            byte[] result = new byte[data.Length * 2];

            for (int i = 0; i < data.Length; i++) {
                result[i + 0] = Convert.ToByte((data[i] >> 0) & 0xff);
                result[i + 1] = Convert.ToByte((data[i] >> 8) & 0xff);
            }

            return result;
        }
    }
}
