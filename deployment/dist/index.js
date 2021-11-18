"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const ExternalStack_1 = require("./stacks/ExternalStack");
const AppStack_1 = require("./stacks/AppStack");
const aws_cdk_lib_1 = require("aws-cdk-lib");
const app = new aws_cdk_lib_1.App();
new ExternalStack_1.ExternalStack(app, "Tsukiy0ExtensionsExternalStack", {
    env: {
        account: process.env.CDK_DEFAULT_ACCOUNT,
        region: "us-east-1",
    },
});
new AppStack_1.AppStack(app, "Tsukiy0ExtensionsAppStack", {
    env: {
        account: process.env.CDK_DEFAULT_ACCOUNT,
        region: "us-east-1",
    },
});
//# sourceMappingURL=index.js.map