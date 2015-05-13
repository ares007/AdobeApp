/** adobe-test
 * a collection of functions for testing some things
 */

#include "adobe.js"

// a main function (used as default if no function given) returns "main"
function main(args) {
	return 'main';
}

// echo returns it's input
function echo(args) {
	return args;
}

// logger writes log messages
function logger(args) {
	log.debug('logger-debug test');
	log.info('logger-info test');
	log.warn('logger-warn test');
	log.error('logger-error test');
}

// croak -- wirft eine Exception
function croak(args) {
	throw new Error('croak died');
}
