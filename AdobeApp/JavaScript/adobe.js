/* adobe.js
 *
 * controlled calling of a JavaScript function
 * initializing and cleanup if available
 *
 */

/** a globally available logger */
var log = {};

/** all our internals are encapsulated */
(function(functionName, jsonArgs, logOptions) {
    function escapeChar(string) {
        var hex = string.charCodeAt(0).toString(16);
        while (hex.length < 4)
            hex = '0' + hex;
        return '\\u' + hex;
    }
    
    function escape(string) {
        return string.replace(/([\u0000-\u001f\"\\\u007f-\uffff])/g, escapeChar);
    }

    // see http://blogs.sitepointstatic.com/examples/tech/json-serialization/json-serialization.js
    // (C) Craig Buckler, Optimalworks.net
    // modified by using escape for strings, renamed some variables
    function toJson(object) {
        var t = typeof(object);
        if (t !== 'object' || object === null) {
            // simple data type
            if (t === 'string')
                return '"' + escape(object) + '"'
            else
                return String(object);
        } else {
            // recurse array or object
            var n;
            var v;
            var json = [];
            var isArray = object && object.constructor == Array;
            
            for (n in object) {
                v = object[n];
                t = typeof(v);
            
                if (t === 'string') 
                    v = '"' + escape(v) + '"'
                else if (t === 'object' && v !== null)
                    v = toJson(v);
            
                json.push((isArray ? '' : '"' + n + '":') + String(v));
            }
            
            return isArray
                ? '[' + String(json) + ']'
                : '{' + String(json) + '}';
        }
    }
    
    function f(functionName) {
        return $.global[functionName];
    }
    
    function invoke(functionName, args) {
        var func = f(functionName);
        if (!func)
            throw new Error('Function "' + functionName + '" does not exist, stopping.');
        return func.call(this, args);
    }
    
    function tryInvokeIfExists(functionName, args) {
        if (!f(functionName)) {
        	log.debug('tryInvoke "' + functionName + '" -- missing, not started');
        	return true;
        }
        
        var success = false;
        
        try {
        	log.debug('tryInvoke "' + functionName + '" -- starting');
            invoke(function_name, args);

        	log.debug('tryInvoke "' + functionName + '" -- done');
        	success = true;
        } catch(e) {
        	log.debug('tryInvoke "' + functionName + '" -- failed with Exception "' + e.name + '"');
        }
        
        return success;
    }

    function initializeLogger(logOptions, response) {
    	var logLine = function() {};

    	if (logOptions === 'array') {
    		logLine = function(severity, message) {
    			response.log.push({ severity: severity, message: message });
    	    }
    	} // evtl. Datei-Logging

    	log.debug = function(message) { logLine('debug', message); }
    	log.info  = function(message) { logLine('info', message); }
    	log.warn  = function(message) { logLine('warn', message); }
    	log.error = function(message) { logLine('error', message); }
    }

    // main entry point
    var response = {
    	success: false,
    	result: {},
    	log: [],
    	exception: undefined
    };

    initializeLogger(logOptions, response);

   	log.info('JavaScript starting');
    try {
        tryInvokeIfExists('autoInit');

        log.debug('invoke "' + functionName + '"');
        var args = eval('(' + jsonArgs + ')');
        var result = invoke(functionName, args);

        tryInvokeIfExists('autoExit');

        response.result = result;
        response.success = true;
    } catch(e) {
        log.debug('caught exception "' + e.name + '"');
        response.exception = {
         	name: e.name,
         	description: e.description,
         	line: e.line,
         	fileName: e.fileName
        };
    }

    // must return a string which travels thru AppleScript
    log.info('JavaScript done');
    return toJson(response);
})(
    arguments[0] || 'main', // function to call
    arguments[1] || '{}',   // arguments as JSON string
    arguments[2]			// log options
);
