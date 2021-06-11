import * as cdk from "@aws-cdk/core";
import { Root } from "./Root";

const app = new cdk.App();
new Root(app, "Tsukiy0Extensions", {
  env: {
    account: process.env.CDK_DEFAULT_ACCOUNT!,
    region: process.env.CDK_DEFAULT_REGION,
  },
});
