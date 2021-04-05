namespace WinTail
{
    #region Neutral/system messages

    /// <summary>
    /// Marker class to continue processing.
    /// </summary>
    public record ContinueProcessing;

    #endregion

    #region Success messages

    /// <summary>
    /// Base class for signalling that user input was valid.
    /// </summary>
    public record InputSuccess(string Reason);

    #endregion

    #region Error messages

    /// <summary>
    /// Base class for signalling that user input was invalid.
    /// </summary>
    public record InputError(string Reason);

    /// <summary>
    /// User provided blank input.
    /// </summary>
    public record NullInputError(string Reason) : InputError(Reason);

    /// <summary>
    /// User provided invalid input (currently, input // odd # chars)
    /// </summary>
    public record ValidationError(string Reason) : InputError(Reason);

    #endregion
}
