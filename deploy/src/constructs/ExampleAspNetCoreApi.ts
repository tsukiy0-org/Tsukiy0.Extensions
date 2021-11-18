import path from "path";
import { Construct } from "constructs";
import { Duration } from "aws-cdk-lib";
import {
  DockerImageFunction,
  DockerImageCode,
} from "aws-cdk-lib/lib/aws-lambda";
import { RetentionDays } from "aws-cdk-lib/lib/aws-logs";
import { DefaultLambdaRestApi } from "@tsukiy0/extensions-aws-cdk";
import { External } from "./External";

export class ExampleAspNetCoreApi extends Construct {
  constructor(
    scope: Construct,
    id: string,
    props: {
      external: External;
    }
  ) {
    super(scope, id);

    const fn = new DockerImageFunction(this, "Function", {
      code: DockerImageCode.fromImageAsset(
        path.resolve(
          __dirname,
          "../../../Tsukiy0.Extensions.Example.AspNetCore"
        )
      ),
      memorySize: 512,
      timeout: Duration.seconds(30),
      logRetention: RetentionDays.ONE_WEEK,
      retryAttempts: 0,
    });
    props.external.grantReadParam(
      fn,
      "tsukiy0/extensions/aspnetcore/apikey/service"
    );

    new DefaultLambdaRestApi(this, "Api", {
      fn,
    });
  }
}
