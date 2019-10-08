namespace ReLearn.Core.Provider
{
    /// <summary>
    ///     Взаимодействие с внешними приложениями.
    /// </summary>
    public interface IInteractionProvider
    {
        /// <summary>
        ///     Поддерживает ли внешнее приложение набор номера.
        /// </summary>
        /// <returns><c>true</c>, если поддерживается, <c>false</c> иначе.</returns>
        bool IsApplicationCallSupported();

        /// <summary>
        ///     Открыть внешнее приложение с набранным номером отправителя.
        /// </summary>
        /// <param name="phoneNumber">Номер отправителя.</param>
        void OpenCallApplication(string phoneNumber);
    }
}
