/** starter.js
 * contains some functions for testing
 */

#include "adobe.js"

function main(args) {
	return { end: "main", value: 42, arg: args };
}

function croak(args) {
    throw new Error("croak died intentionally");
}