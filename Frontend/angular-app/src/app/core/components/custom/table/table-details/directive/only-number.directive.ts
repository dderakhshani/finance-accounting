import {Directive, HostBinding, HostListener} from '@angular/core';

@Directive({
  selector: '[appOnlyNumber]'
})
export class OnlyNumberDirective {
  @HostBinding('style.direction') direction = 'ltr';
  constructor() {}

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): boolean {
    const inputElement = event.target as HTMLInputElement;
    const currentValue = inputElement.value;
    const charCode = event.key;

    // اجازه دادن به کلیدهای ترکیبی (Ctrl/Cmd + ...)
    if (event.ctrlKey || event.metaKey) {
      return true;
    }

    // کلیدهای مجاز
    const allowedKeys = [
      'Backspace',
      'Tab',
      'ArrowLeft',
      'ArrowRight',
      '.',
      'Delete',
      'Enter'
    ];


    if (allowedKeys.includes(charCode)) {

      if (charCode === '.') {
        const currentValueWithoutCommas = currentValue.replace(/,/g, '');
        if (currentValueWithoutCommas.includes('.')) {
          event.preventDefault();
          return false;
        }
      }
      return true;
    }


    if (charCode === '-') {
      if (
        currentValue.includes('-') ||
        inputElement.selectionStart !== 0
      ) {
        event.preventDefault();
        return false;
      }
      return true;
    }


    if (/^[0-9]$/.test(charCode)) {
      return true;
    }

    event.preventDefault();
    return false;
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent): void {
    event.preventDefault();
    const clipboardData = event.clipboardData;
    const pastedData = clipboardData ? clipboardData.getData('text/plain') : '';


    let sanitized = pastedData.replace(/[^-0-9.]/g, '');


    const hasNegative = sanitized.startsWith('-');
    sanitized = sanitized.replace(/-/g, '');
    if (hasNegative) {
      sanitized = '-' + sanitized;
    }

    // مدیریت نقاط اعشاری تکراری
    const parts = sanitized.split('.');
    if (parts.length > 2) {
      sanitized = parts[0] + '.' + parts.slice(1).join('');
    }

    // فرمت و اعمال مقدار
    const inputElement = event.target as HTMLInputElement;
    inputElement.value = this.formatNumberWithCommas(sanitized);
  }

  @HostListener('input', ['$event'])
  onInput(event: InputEvent): void {
    const inputElement = event.target as HTMLInputElement;
    const currentValue = inputElement.value;
    inputElement.value = this.formatNumberWithCommas(currentValue);
  }

  private formatNumberWithCommas(value: string): string {
    let isNegative = false;
    if (value.startsWith('-')) {
      isNegative = true;
      value = value.substring(1);
    }

    // حذف تمام غیراعداد و نقاط اضافی
    let formattedValue = value.replace(/[^0-9.]/g, '');
    const parts = formattedValue.split('.');
    if (parts.length > 1) {
      formattedValue = parts[0] + '.' + parts.slice(1).join('');
    }

    // افزودن ویرگول به عنوان جداکننده هزارگان
    formattedValue = formattedValue.replace(/\B(?=(\d{3})+(?!\d))/g, ',');

    // بازگردانی علامت منفی
    if (isNegative) {
      formattedValue = '-' + formattedValue;
    }

    return formattedValue;
  }
}
