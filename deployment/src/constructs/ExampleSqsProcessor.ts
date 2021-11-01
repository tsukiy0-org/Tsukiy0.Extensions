import path from "path";
import { Construct } from "constructs";
import { Duration } from "aws-cdk-lib";
import { DockerImageCode } from "aws-cdk-lib/lib/aws-lambda";
import { External } from "./External";
import {
  DefaultDockerFunction,
  FunctionQueue,
} from "@tsukiy0/extensions-aws-cdk";
import { IParameter, StringParameter } from "aws-cdk-lib/lib/aws-ssm";
import { IQueue } from "aws-cdk-lib/lib/aws-sqs";
import { IGrantable } from "aws-cdk-lib/lib/aws-iam";

export class ExampleSqsProcessor extends Construct {
  private readonly queueUrlParam: IParameter;
  private readonly queue: IQueue;

  constructor(
    scope: Construct,
    id: string,
    props: {
      external: External;
    }
  ) {
    super(scope, id);

    const timeout = Duration.seconds(30);
    const fn = new DefaultDockerFunction(this, "Function", {
      code: DockerImageCode.fromImageAsset(
        path.resolve(
          __dirname,
          "../../../Tsukiy0.Extensions.Example.Processor.Aws.Sqs"
        )
      ),
      memorySize: 512,
    });
    props.external.grantTableReadWrite(fn);

    const fnQueue = new FunctionQueue(this, "FunctionQueue", {
      fn: fn,
      timeout,
      maxAttempts: 1,
    });

    const queueUrlParam = new StringParameter(this, "QueueUrl", {
      parameterName: "/tsukiy0/extensions/sqs-processor/queue-url",
      stringValue: fnQueue.queue.queue.queueUrl,
    });

    this.queueUrlParam = queueUrlParam;
    this.queue = fnQueue.queue.queue;
  }

  public grantSubmit = (grantee: IGrantable): void => {
    this.queueUrlParam.grantRead(grantee);
    this.queue.grantSendMessages(grantee);
  };
}
