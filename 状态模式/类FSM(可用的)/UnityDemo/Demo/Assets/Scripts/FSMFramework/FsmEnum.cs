namespace FSM
{
    /// <summary>
    /// 状态名字 或状态机名字
    /// </summary>
    public enum StateName
    {
        Null,
        MainMachine,
        Close,
        OpenMachine,
        Intensity,
        Color
    }
    /// <summary>
    /// 状态Tag 或 状态机Tag
    /// </summary>
    public enum StateTag
    {
        Null
    }
    /// <summary>
    /// 状态过渡名字
    /// </summary>
    public enum TransitionName
    {
        Null,
        Close2Open,
        Color2Intensity,
        Intensity2Color,
        Open2Close
    }
    

   
}
