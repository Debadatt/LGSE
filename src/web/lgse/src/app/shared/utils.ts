import { FormControl } from '@angular/forms';

export default class Utils {

  static validateWorkEmail(control: FormControl): { [s: string]: boolean } {
    let email = control.value;
    if (email && email.indexOf("@") != -1) {
      let [_, domain] = email.split("@");
      if (domain !== "wandw.com" || domain !== "gmail.com") {
        return { 'invalidWorkEmailId': true }
      }
    }
    return null;
  }

}