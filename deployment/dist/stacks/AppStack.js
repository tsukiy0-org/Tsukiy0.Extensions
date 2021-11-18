"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppStack = void 0;
const aws_cdk_lib_1 = require("aws-cdk-lib");
const ExampleAspNetCoreApi_1 = require("../constructs/ExampleAspNetCoreApi");
const External_1 = require("../constructs/External");
class AppStack extends aws_cdk_lib_1.Stack {
    constructor(scope, id, props) {
        super(scope, id, props);
        const external = new External_1.External(this, "External", {
            stack: this,
        });
        new ExampleAspNetCoreApi_1.ExampleAspNetCoreApi(this, "ExampleAspNetCoreApi", {
            external,
        });
    }
}
exports.AppStack = AppStack;
//# sourceMappingURL=AppStack.js.map