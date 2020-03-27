/****************************************************
    文件：PETools.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/2/29 12:1:36
	功能：工具类
*****************************************************/
public class PETools 
{
    // 包括min和max
    public static int RDInt(int min, int max, System.Random rd = null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }
        int val = rd.Next(min, max + 1);
        return val;
    }
}