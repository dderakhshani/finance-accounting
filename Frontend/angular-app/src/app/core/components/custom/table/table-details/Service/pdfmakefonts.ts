// @ts-ignore
export class CustomFont {
  static Font = {
    fontFace: `
      @font-face {
        font-family: 'IranYekanBold';
        src: url('/assets/fonts/iranyekanwebboldfanum.ttf') format('truetype');
        font-display: swap;
      }

      @font-face {
        font-family: 'IranYekanBold';
        src: url("../../fonts/iranyekanwebboldfanum.ttf") format('truetype');
        font-display: swap;
      }

      @font-face {
        font-family: 'IranYekanExtraBold';
        src: url("../../fonts/iranyekanwebextraboldfanum.ttf") format('truetype');
        font-display: swap;
      }

      @font-face {
        font-family: "iran_sans";
        src: url("/assets/fonts/iran-sans/IRANSansWeb(FaNum).eot"); /* IE9 Compat Modes */
        src: url("/assets/fonts/iran-sans/IRANSansWeb(FaNum).eot?#iefix") format("embedded-opentype"),
             url("/assets/fonts/iran-sans/IRANSansWeb(FaNum).woff2") format("woff2"),
             url("/assets/fonts/iran-sans/IRANSansWeb(FaNum).woff") format("woff"),
             url("/assets/fonts/iran-sans/IRANSansWeb(FaNum).ttf") format("truetype"); /* Safari, Android, iOS */
        font-display: swap;
      }

      @font-face {
        font-family: 'Vazir';
        src: url("/assets/fonts/Vazir/Vazir.eot"); /* IE9 Compat Modes */
        src: url("/assets/fonts/Vazir/Vazir.eot?#iefix") format("embedded-opentype"),
             url("/assets/fonts/Vazir/Vazir.woff2") format("woff2"),
             url("/assets/fonts/Vazir/Vazir.woff") format("woff"),
             url("/assets/fonts/Vazir/Vazir.ttf") format("truetype"); /* Safari, Android, iOS */
        font-display: swap;
      }

      @font-face {
        font-family: 'BNazanin';
        src: url("/assets/fonts/BNazanin/BNazanin.eot"); /* IE9 Compat Modes */
        src: url("/assets/fonts/BNazanin/BNazanin.eot?#iefix") format("embedded-opentype"),
             url("/assets/fonts/BNazanin/BNazanin.woff2") format("woff2"),
             url("/assets/fonts/BNazanin/BNazanin.woff") format("woff"),
             url("/assets/fonts/BNazanin/BNazanin.ttf") format("truetype");
        font-display: swap;
      }
    `
  };
}
