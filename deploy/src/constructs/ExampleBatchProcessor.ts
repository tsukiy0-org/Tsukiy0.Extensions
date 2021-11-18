import { Construct } from "constructs";
import path from "path";
import { FargateBatchJob } from "@tsukiy0/extensions-aws-cdk";
import { DockerImageAsset } from "aws-cdk-lib/lib/aws-ecr-assets";
import { IParameter, StringParameter } from "aws-cdk-lib/lib/aws-ssm";
import { IGrantable } from "aws-cdk-lib/lib/aws-iam";
import { External } from "./External";

export class ExampleBatchProcessor extends Construct {
  private readonly job: FargateBatchJob;
  private readonly jobQueueArnParam: IParameter;
  private readonly jobDefinitionArnParam: IParameter;

  public constructor(
    scope: Construct,
    id: string,
    props: {
      external: External;
    }
  ) {
    super(scope, id);

    const job = new FargateBatchJob(this, "Job", {
      vpc: props.external.vpc,
      dockerImage: new DockerImageAsset(this, "JobImage", {
        directory: path.resolve(
          __dirname,
          "../../../Tsukiy0.Extensions.Example.Processor.Aws.Batch"
        ),
      }),
      computeResources: {
        maxvCpus: 1,
      },
      jobDefinition: {
        resourceRequirements: {
          mem: 1024,
          vcpu: 0.5,
        },
        environment: [],
      },
    });
    props.external.grantTableReadWrite(job.role);

    const jobDefinitionArnParam = new StringParameter(
      this,
      "JobDefinitionArn",
      {
        parameterName: "/tsukiy0/extensions/batch-processor/job-definition-arn",
        stringValue: job.jobDefinition.ref,
      }
    );

    const jobQueueArnParam = new StringParameter(this, "JobQueueArn", {
      parameterName: "/tsukiy0/extensions/batch-processor/job-queue-arn",
      stringValue: job.jobQueue.ref,
    });

    this.job = job;
    this.jobDefinitionArnParam = jobDefinitionArnParam;
    this.jobQueueArnParam = jobQueueArnParam;
  }

  public grantSubmit = (grantee: IGrantable): void => {
    this.jobDefinitionArnParam.grantRead(grantee);
    this.jobQueueArnParam.grantRead(grantee);
    this.job.grantSubmit(grantee);
  };
}
