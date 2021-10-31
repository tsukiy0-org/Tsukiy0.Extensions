import { Stack, StackProps } from "aws-cdk-lib";
import { Construct } from "constructs";
import { ExampleAspNetCoreApi } from "../constructs/ExampleAspNetCoreApi";
import { ExampleSqsProcessor } from "../constructs/ExampleSqsProcessor";
import { External } from "../constructs/External";

export class AppStack extends Stack {
  constructor(
    scope: Construct,
    id: string,
    props: StackProps & {
      tableName: string;
    }
  ) {
    super(scope, id, props);

    const external = new External(this, "External", {
      stack: this,
      tableName: props.tableName,
    });

    new ExampleAspNetCoreApi(this, "ExampleAspNetCoreApi", {
      external,
    });

    new ExampleSqsProcessor(this, "ExampleSqsProcessor", {
      external,
    });
  }
}
