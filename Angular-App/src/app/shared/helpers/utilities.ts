export default class Utilities {
  public static ConvertDateBeforeSendToServer(dateTimeString) {
    const dateTimeObject = new Date(dateTimeString);
    return new Date(dateTimeObject.getTime() - dateTimeObject.getTimezoneOffset() * 60000);
  }
}
