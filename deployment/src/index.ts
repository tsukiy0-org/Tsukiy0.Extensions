import { ExternalStack } from "./stacks/ExternalStack";
import { AppStack } from "./stacks/AppStack";
import { App } from "aws-cdk-lib";

const app = new App();

new ExternalStack(app, "Tsukiy0ExtensionsExternalStack", {
  env: {
    account: process.env.CDK_DEFAULT_ACCOUNT!,
    region: "us-east-1",
  },
});

new AppStack(app, "Tsukiy0ExtensionsAppStack", {
  env: {
    account: process.env.CDK_DEFAULT_ACCOUNT!,
    region: "us-east-1",
  },
});
