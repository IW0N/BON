using System.Security.Cryptography;
using System.Text;

namespace Tson;
internal class Free2PId
{
    const long TICKS_PER_MILLISEC = 10_000;
    const long TICKS_PER_SECOND = TICKS_PER_MILLISEC*1000;
    const long TICKS_PER_HOUR = TICKS_PER_SECOND * 3600;
    readonly byte[] static_bytes;
    readonly byte[] static_hash;
    byte[] temp_bytes;
    string temp_string;
    public string Static { get; }
    public string Temporary { get=>temp_string; }
    bool is_first = true;
    public Free2PId(string static_string)
    {
        Static = static_string;
        static_bytes = Encoding.UTF8.GetBytes(static_string);
        static_hash = SHA512.HashData(static_bytes);
        UpdateTemporary();
        LaunchTemporaryUpdating();
    }
    private void LaunchTemporaryUpdating()
    {
        Task.Run(() =>
        {
            while (true)
            {
                DateTime now = DateTime.UtcNow;
                long hour_period = GetCurrentHourTicks(now);
                if(!is_first)
                    UpdateTemporary(hour_period);
                long from_current_hour = now.Ticks-hour_period;
                long ticks_to_next_hour = TICKS_PER_HOUR-from_current_hour;
                int millis_to_next =(int)(ticks_to_next_hour/TICKS_PER_MILLISEC);
                Task.Delay(millis_to_next).Wait();
                is_first = false;
            }
        });
    }
    private long GetCurrentHourTicks(DateTime now)
    {
        long ticks = now.Ticks;
        long hour_period = ticks - ticks % TICKS_PER_HOUR;
        return hour_period;
    }
    private void UpdateTemporary()
    {
        long ticks = GetCurrentHourTicks(DateTime.UtcNow);
        UpdateTemporary(ticks);
    }
    private void UpdateTemporary(long hour_period)
    {
        byte[] bts = BitConverter.GetBytes(hour_period);
        int bts_len = bts.Length;
        int hash_len = static_hash.Length;
        byte[] temp_address_seed = new byte[bts_len + hash_len];
        Buffer.BlockCopy(bts, 0, temp_address_seed, 0, bts_len);
        Buffer.BlockCopy(static_hash, 0, temp_address_seed, bts_len, hash_len);

        byte[] temp_seed=SHA512.HashData(temp_address_seed);
        int count = 0;
        foreach (byte b in temp_seed)
            count += b%100;
        Console.WriteLine(count);
        for(int x=0;x<count;x++)
            temp_seed=SHA512.HashData(temp_seed);
        
        temp_bytes = temp_seed;
        temp_string = Convert.ToHexString(temp_bytes).ToLower();
    }
    public override string ToString()
    {
        return temp_string;
    }
}
