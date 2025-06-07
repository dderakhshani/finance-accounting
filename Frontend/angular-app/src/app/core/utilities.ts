//TODO: This Files is shared between Guest & Admin section
export class Utilities {


    public static deepCopy<T>(obj: T): T {
        let copy: any;

        // Handle the 3 simple types, and null or undefined
        if (null == obj || "object" !== typeof obj) {
            return obj;
        }

        // Handle Date
        if (obj instanceof Date) {
            copy = new Date();
            copy.setTime(obj.getTime());
            return <any>copy;
        }

        // Handle Array
        if (obj instanceof Array) {
            copy = [];
            for (let i = 0, len = obj.length; i < len; i++) {
                copy[i] = Utilities.deepCopy(obj[i]);
            }
            return <any>copy;
        }

        // Handle Set
        if (obj instanceof Set) {
            copy = new Set();
            obj.forEach(value => {
                copy.add(Utilities.deepCopy(value))
            });
            // for (let i = 0, len = obj.length; i < len; i++) {
            //     copy[i] = Utilities.deepCopy(obj[i]);
            // }
            return <any>copy;
        }

        // Handle Object
        if (obj instanceof Object) {
            copy = {};
            for (const attr in obj) {
                if ((<any>obj).hasOwnProperty(attr)) {
                    (<any>copy)[attr] = Utilities.deepCopy(obj[attr]);
                }
            }
            return <any>copy;
        }

        //throw new Error("Unable to copy obj! Its type isn't supported.");
        return obj;
    }

    public static nameOf<T>(key: keyof T): string
    public static nameOf<T, K>(key: keyof T, key2?: keyof K): string
    public static nameOf<T, K, U>(key: keyof T, key2?: keyof K, key3?: keyof U): string {
        let result = key.toString();
        if (key2) {
            result += `.${key2.toString()}`;
        }
        if (key3) {
            result += `.${key3.toString()}`;
        }
        return result;
    }


    public static addDays(numOfDays: number, date: Date) {
        if (!date) {
            return date;
        }
        //The Date object automatically takes care of rolling over the month and year if adding X days to a date pushes us into the next month or year.
        date.setDate(date.getDate() + numOfDays);

        return date;
    }

    public static zeroUTC(date: Date) {
        // or--
        // var d = new Date();
        // d.setUTCHours(0, 0, 0, 0);
        const off = date.getTimezoneOffset()
        const absoff = Math.abs(off)
        return new Date(date.getTime() - off * 60 * 1000);
    }

    public static dateToLocalISO(date: Date) {
        const off = date.getTimezoneOffset()
        const absoff = Math.abs(off)
        return new Date(date.getTime() - off * 60 * 1000).toISOString().substring(0, 23)
    }
}