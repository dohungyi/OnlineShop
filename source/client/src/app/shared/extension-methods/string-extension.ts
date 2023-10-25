interface String {
    /**
     * Return true if null or empty, otherwise false.
     */
    isEmpty(): boolean;

    /**
     * Return true if value is valid mail, otherwise false.
     */
    isMail(): boolean;
}

/**
 * Return true if empty, otherwise false.
 */
String.prototype.isEmpty = function () {
    if (!this || (this as string).length === 0) {
        return true;
    }
    return false;
}

/**
 * Return true if value is valid mail, otherwise false.
 */
String.prototype.isMail = function () {
    if (this.isEmpty())
        return false;

    const re = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    return re.test(this.toLocaleLowerCase());
}
