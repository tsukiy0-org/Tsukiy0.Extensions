import path from "path";
import { Construct } from "constructs";
import { Duration } from "aws-cdk-lib";
import { DockerImageCode } from "aws-cdk-lib/lib/aws-lambda";
import { External } from "./External";
import {
  DefaultDockerFunction,
  DefaultFunctionHttpApi,
} from "@tsukiy0/aws-cdk-tools";

export class ExampleAspNetCoreApi extends Construct {
  constructor(
    scope: Construct,
    id: string,
    props: {
      external: External;
    }
  ) {
    super(scope, id);

    // const fn = new DefaultDockerFunction(this, "Function", {
    //   code: DockerImageCode.fromImageAsset(
    //     path.resolve(
    //       __dirname,
    //       "../../../Tsukiy0.Extensions.Example.AspNetCore"
    //     )
    //   ),
    //   memorySize: 512,
    //   timeout: Duration.seconds(30),
    // });
    // props.external.grantReadParam(
    //   fn,
    //   "tsukiy0/extensions/aspnetcore/apikey/service"
    // );

    // new DefaultFunctionHttpApi(this, "Api", {
    //   fn,
    // });
  }
}
