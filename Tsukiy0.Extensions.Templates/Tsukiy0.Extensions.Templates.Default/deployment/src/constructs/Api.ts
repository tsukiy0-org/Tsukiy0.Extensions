import path from "path";
import { Construct } from "constructs";
import { Duration } from "aws-cdk-lib";
import { DefaultDockerFunction, DefaultFunctionHttpApi } from "@tsukiy0/aws-cdk-tools";
import { StringParameter } from "aws-cdk-lib/aws-ssm";
import { DockerImageCode } from "aws-cdk-lib/aws-lambda";

export class Api extends Construct {
  constructor(
    scope: Construct,
    id: string,
  ) {
    super(scope, id);

    const fn = new DefaultDockerFunction(this, "Function", {
      code: DockerImageCode.fromImageAsset(
        path.resolve(__dirname, "../../../Tsukiy0.Extensions.Templates.Default.Api")
      ),
      memorySize: 512,
      timeout: Duration.seconds(30),
    });

    const api = new DefaultFunctionHttpApi(this, "Api", {
      fn,
    });

    new StringParameter(this, "ApiUrl", {
      parameterName: "/api/url",
      stringValue: api.url!,
    });
  }
}
