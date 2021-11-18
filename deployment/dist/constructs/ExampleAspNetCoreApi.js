"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ExampleAspNetCoreApi = void 0;
const path_1 = __importDefault(require("path"));
const constructs_1 = require("constructs");
const aws_cdk_lib_1 = require("aws-cdk-lib");
const aws_lambda_1 = require("aws-cdk-lib/lib/aws-lambda");
const aws_logs_1 = require("aws-cdk-lib/lib/aws-logs");
const extensions_aws_cdk_1 = require("@tsukiy0/extensions-aws-cdk");
class ExampleAspNetCoreApi extends constructs_1.Construct {
    constructor(scope, id, props) {
        super(scope, id);
        const fn = new aws_lambda_1.DockerImageFunction(this, "Function", {
            code: aws_lambda_1.DockerImageCode.fromImageAsset(path_1.default.resolve(__dirname, "../../../../Tsukiy0.Extensions.Example.AspNetCore")),
            memorySize: 512,
            timeout: aws_cdk_lib_1.Duration.seconds(30),
            logRetention: aws_logs_1.RetentionDays.ONE_WEEK,
            retryAttempts: 0,
        });
        props.external.grantReadParam(fn, "tsukiy0/extensions/aspnetcore/apikey/service");
        new extensions_aws_cdk_1.DefaultLambdaRestApi(this, "Api", {
            fn,
        });
    }
}
exports.ExampleAspNetCoreApi = ExampleAspNetCoreApi;
//# sourceMappingURL=ExampleAspNetCoreApi.js.map