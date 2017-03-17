// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: excel_pb_desc.proto

package lit_frame.config;

public final class ExcelPbDesc {
  private ExcelPbDesc() {}
  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistryLite registry) {
  }

  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistry registry) {
    registerAllExtensions(
        (com.google.protobuf.ExtensionRegistryLite) registry);
  }
  public interface hero_skillOrBuilder extends
      // @@protoc_insertion_point(interface_extends:lit_frame.config.hero_skill)
      com.google.protobuf.MessageOrBuilder {

    /**
     * <code>uint32 type_id = 1;</code>
     */
    int getTypeId();

    /**
     * <code>string display_name = 2;</code>
     */
    java.lang.String getDisplayName();
    /**
     * <code>string display_name = 2;</code>
     */
    com.google.protobuf.ByteString
        getDisplayNameBytes();
  }
  /**
   * Protobuf type {@code lit_frame.config.hero_skill}
   */
  public  static final class hero_skill extends
      com.google.protobuf.GeneratedMessageV3 implements
      // @@protoc_insertion_point(message_implements:lit_frame.config.hero_skill)
      hero_skillOrBuilder {
    // Use hero_skill.newBuilder() to construct.
    private hero_skill(com.google.protobuf.GeneratedMessageV3.Builder<?> builder) {
      super(builder);
    }
    private hero_skill() {
      typeId_ = 0;
      displayName_ = "";
    }

    @java.lang.Override
    public final com.google.protobuf.UnknownFieldSet
    getUnknownFields() {
      return com.google.protobuf.UnknownFieldSet.getDefaultInstance();
    }
    private hero_skill(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      this();
      int mutable_bitField0_ = 0;
      try {
        boolean done = false;
        while (!done) {
          int tag = input.readTag();
          switch (tag) {
            case 0:
              done = true;
              break;
            default: {
              if (!input.skipField(tag)) {
                done = true;
              }
              break;
            }
            case 8: {

              typeId_ = input.readUInt32();
              break;
            }
            case 18: {
              java.lang.String s = input.readStringRequireUtf8();

              displayName_ = s;
              break;
            }
          }
        }
      } catch (com.google.protobuf.InvalidProtocolBufferException e) {
        throw e.setUnfinishedMessage(this);
      } catch (java.io.IOException e) {
        throw new com.google.protobuf.InvalidProtocolBufferException(
            e).setUnfinishedMessage(this);
      } finally {
        makeExtensionsImmutable();
      }
    }
    public static final com.google.protobuf.Descriptors.Descriptor
        getDescriptor() {
      return lit_frame.config.ExcelPbDesc.internal_static_lit_frame_config_hero_skill_descriptor;
    }

    protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
        internalGetFieldAccessorTable() {
      return lit_frame.config.ExcelPbDesc.internal_static_lit_frame_config_hero_skill_fieldAccessorTable
          .ensureFieldAccessorsInitialized(
              lit_frame.config.ExcelPbDesc.hero_skill.class, lit_frame.config.ExcelPbDesc.hero_skill.Builder.class);
    }

    public static final int TYPE_ID_FIELD_NUMBER = 1;
    private int typeId_;
    /**
     * <code>uint32 type_id = 1;</code>
     */
    public int getTypeId() {
      return typeId_;
    }

    public static final int DISPLAY_NAME_FIELD_NUMBER = 2;
    private volatile java.lang.Object displayName_;
    /**
     * <code>string display_name = 2;</code>
     */
    public java.lang.String getDisplayName() {
      java.lang.Object ref = displayName_;
      if (ref instanceof java.lang.String) {
        return (java.lang.String) ref;
      } else {
        com.google.protobuf.ByteString bs = 
            (com.google.protobuf.ByteString) ref;
        java.lang.String s = bs.toStringUtf8();
        displayName_ = s;
        return s;
      }
    }
    /**
     * <code>string display_name = 2;</code>
     */
    public com.google.protobuf.ByteString
        getDisplayNameBytes() {
      java.lang.Object ref = displayName_;
      if (ref instanceof java.lang.String) {
        com.google.protobuf.ByteString b = 
            com.google.protobuf.ByteString.copyFromUtf8(
                (java.lang.String) ref);
        displayName_ = b;
        return b;
      } else {
        return (com.google.protobuf.ByteString) ref;
      }
    }

    private byte memoizedIsInitialized = -1;
    public final boolean isInitialized() {
      byte isInitialized = memoizedIsInitialized;
      if (isInitialized == 1) return true;
      if (isInitialized == 0) return false;

      memoizedIsInitialized = 1;
      return true;
    }

    public void writeTo(com.google.protobuf.CodedOutputStream output)
                        throws java.io.IOException {
      if (typeId_ != 0) {
        output.writeUInt32(1, typeId_);
      }
      if (!getDisplayNameBytes().isEmpty()) {
        com.google.protobuf.GeneratedMessageV3.writeString(output, 2, displayName_);
      }
    }

    public int getSerializedSize() {
      int size = memoizedSize;
      if (size != -1) return size;

      size = 0;
      if (typeId_ != 0) {
        size += com.google.protobuf.CodedOutputStream
          .computeUInt32Size(1, typeId_);
      }
      if (!getDisplayNameBytes().isEmpty()) {
        size += com.google.protobuf.GeneratedMessageV3.computeStringSize(2, displayName_);
      }
      memoizedSize = size;
      return size;
    }

    private static final long serialVersionUID = 0L;
    @java.lang.Override
    public boolean equals(final java.lang.Object obj) {
      if (obj == this) {
       return true;
      }
      if (!(obj instanceof lit_frame.config.ExcelPbDesc.hero_skill)) {
        return super.equals(obj);
      }
      lit_frame.config.ExcelPbDesc.hero_skill other = (lit_frame.config.ExcelPbDesc.hero_skill) obj;

      boolean result = true;
      result = result && (getTypeId()
          == other.getTypeId());
      result = result && getDisplayName()
          .equals(other.getDisplayName());
      return result;
    }

    @java.lang.Override
    public int hashCode() {
      if (memoizedHashCode != 0) {
        return memoizedHashCode;
      }
      int hash = 41;
      hash = (19 * hash) + getDescriptor().hashCode();
      hash = (37 * hash) + TYPE_ID_FIELD_NUMBER;
      hash = (53 * hash) + getTypeId();
      hash = (37 * hash) + DISPLAY_NAME_FIELD_NUMBER;
      hash = (53 * hash) + getDisplayName().hashCode();
      hash = (29 * hash) + unknownFields.hashCode();
      memoizedHashCode = hash;
      return hash;
    }

    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(
        com.google.protobuf.ByteString data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(
        com.google.protobuf.ByteString data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(byte[] data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(
        byte[] data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(java.io.InputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(
        java.io.InputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input, extensionRegistry);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseDelimitedFrom(java.io.InputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseDelimitedWithIOException(PARSER, input);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseDelimitedFrom(
        java.io.InputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseDelimitedWithIOException(PARSER, input, extensionRegistry);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(
        com.google.protobuf.CodedInputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input);
    }
    public static lit_frame.config.ExcelPbDesc.hero_skill parseFrom(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input, extensionRegistry);
    }

    public Builder newBuilderForType() { return newBuilder(); }
    public static Builder newBuilder() {
      return DEFAULT_INSTANCE.toBuilder();
    }
    public static Builder newBuilder(lit_frame.config.ExcelPbDesc.hero_skill prototype) {
      return DEFAULT_INSTANCE.toBuilder().mergeFrom(prototype);
    }
    public Builder toBuilder() {
      return this == DEFAULT_INSTANCE
          ? new Builder() : new Builder().mergeFrom(this);
    }

    @java.lang.Override
    protected Builder newBuilderForType(
        com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
      Builder builder = new Builder(parent);
      return builder;
    }
    /**
     * Protobuf type {@code lit_frame.config.hero_skill}
     */
    public static final class Builder extends
        com.google.protobuf.GeneratedMessageV3.Builder<Builder> implements
        // @@protoc_insertion_point(builder_implements:lit_frame.config.hero_skill)
        lit_frame.config.ExcelPbDesc.hero_skillOrBuilder {
      public static final com.google.protobuf.Descriptors.Descriptor
          getDescriptor() {
        return lit_frame.config.ExcelPbDesc.internal_static_lit_frame_config_hero_skill_descriptor;
      }

      protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
          internalGetFieldAccessorTable() {
        return lit_frame.config.ExcelPbDesc.internal_static_lit_frame_config_hero_skill_fieldAccessorTable
            .ensureFieldAccessorsInitialized(
                lit_frame.config.ExcelPbDesc.hero_skill.class, lit_frame.config.ExcelPbDesc.hero_skill.Builder.class);
      }

      // Construct using lit_frame.config.ExcelPbDesc.hero_skill.newBuilder()
      private Builder() {
        maybeForceBuilderInitialization();
      }

      private Builder(
          com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
        super(parent);
        maybeForceBuilderInitialization();
      }
      private void maybeForceBuilderInitialization() {
        if (com.google.protobuf.GeneratedMessageV3
                .alwaysUseFieldBuilders) {
        }
      }
      public Builder clear() {
        super.clear();
        typeId_ = 0;

        displayName_ = "";

        return this;
      }

      public com.google.protobuf.Descriptors.Descriptor
          getDescriptorForType() {
        return lit_frame.config.ExcelPbDesc.internal_static_lit_frame_config_hero_skill_descriptor;
      }

      public lit_frame.config.ExcelPbDesc.hero_skill getDefaultInstanceForType() {
        return lit_frame.config.ExcelPbDesc.hero_skill.getDefaultInstance();
      }

      public lit_frame.config.ExcelPbDesc.hero_skill build() {
        lit_frame.config.ExcelPbDesc.hero_skill result = buildPartial();
        if (!result.isInitialized()) {
          throw newUninitializedMessageException(result);
        }
        return result;
      }

      public lit_frame.config.ExcelPbDesc.hero_skill buildPartial() {
        lit_frame.config.ExcelPbDesc.hero_skill result = new lit_frame.config.ExcelPbDesc.hero_skill(this);
        result.typeId_ = typeId_;
        result.displayName_ = displayName_;
        onBuilt();
        return result;
      }

      public Builder clone() {
        return (Builder) super.clone();
      }
      public Builder setField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          Object value) {
        return (Builder) super.setField(field, value);
      }
      public Builder clearField(
          com.google.protobuf.Descriptors.FieldDescriptor field) {
        return (Builder) super.clearField(field);
      }
      public Builder clearOneof(
          com.google.protobuf.Descriptors.OneofDescriptor oneof) {
        return (Builder) super.clearOneof(oneof);
      }
      public Builder setRepeatedField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          int index, Object value) {
        return (Builder) super.setRepeatedField(field, index, value);
      }
      public Builder addRepeatedField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          Object value) {
        return (Builder) super.addRepeatedField(field, value);
      }
      public Builder mergeFrom(com.google.protobuf.Message other) {
        if (other instanceof lit_frame.config.ExcelPbDesc.hero_skill) {
          return mergeFrom((lit_frame.config.ExcelPbDesc.hero_skill)other);
        } else {
          super.mergeFrom(other);
          return this;
        }
      }

      public Builder mergeFrom(lit_frame.config.ExcelPbDesc.hero_skill other) {
        if (other == lit_frame.config.ExcelPbDesc.hero_skill.getDefaultInstance()) return this;
        if (other.getTypeId() != 0) {
          setTypeId(other.getTypeId());
        }
        if (!other.getDisplayName().isEmpty()) {
          displayName_ = other.displayName_;
          onChanged();
        }
        onChanged();
        return this;
      }

      public final boolean isInitialized() {
        return true;
      }

      public Builder mergeFrom(
          com.google.protobuf.CodedInputStream input,
          com.google.protobuf.ExtensionRegistryLite extensionRegistry)
          throws java.io.IOException {
        lit_frame.config.ExcelPbDesc.hero_skill parsedMessage = null;
        try {
          parsedMessage = PARSER.parsePartialFrom(input, extensionRegistry);
        } catch (com.google.protobuf.InvalidProtocolBufferException e) {
          parsedMessage = (lit_frame.config.ExcelPbDesc.hero_skill) e.getUnfinishedMessage();
          throw e.unwrapIOException();
        } finally {
          if (parsedMessage != null) {
            mergeFrom(parsedMessage);
          }
        }
        return this;
      }

      private int typeId_ ;
      /**
       * <code>uint32 type_id = 1;</code>
       */
      public int getTypeId() {
        return typeId_;
      }
      /**
       * <code>uint32 type_id = 1;</code>
       */
      public Builder setTypeId(int value) {
        
        typeId_ = value;
        onChanged();
        return this;
      }
      /**
       * <code>uint32 type_id = 1;</code>
       */
      public Builder clearTypeId() {
        
        typeId_ = 0;
        onChanged();
        return this;
      }

      private java.lang.Object displayName_ = "";
      /**
       * <code>string display_name = 2;</code>
       */
      public java.lang.String getDisplayName() {
        java.lang.Object ref = displayName_;
        if (!(ref instanceof java.lang.String)) {
          com.google.protobuf.ByteString bs =
              (com.google.protobuf.ByteString) ref;
          java.lang.String s = bs.toStringUtf8();
          displayName_ = s;
          return s;
        } else {
          return (java.lang.String) ref;
        }
      }
      /**
       * <code>string display_name = 2;</code>
       */
      public com.google.protobuf.ByteString
          getDisplayNameBytes() {
        java.lang.Object ref = displayName_;
        if (ref instanceof String) {
          com.google.protobuf.ByteString b = 
              com.google.protobuf.ByteString.copyFromUtf8(
                  (java.lang.String) ref);
          displayName_ = b;
          return b;
        } else {
          return (com.google.protobuf.ByteString) ref;
        }
      }
      /**
       * <code>string display_name = 2;</code>
       */
      public Builder setDisplayName(
          java.lang.String value) {
        if (value == null) {
    throw new NullPointerException();
  }
  
        displayName_ = value;
        onChanged();
        return this;
      }
      /**
       * <code>string display_name = 2;</code>
       */
      public Builder clearDisplayName() {
        
        displayName_ = getDefaultInstance().getDisplayName();
        onChanged();
        return this;
      }
      /**
       * <code>string display_name = 2;</code>
       */
      public Builder setDisplayNameBytes(
          com.google.protobuf.ByteString value) {
        if (value == null) {
    throw new NullPointerException();
  }
  checkByteStringIsUtf8(value);
        
        displayName_ = value;
        onChanged();
        return this;
      }
      public final Builder setUnknownFields(
          final com.google.protobuf.UnknownFieldSet unknownFields) {
        return this;
      }

      public final Builder mergeUnknownFields(
          final com.google.protobuf.UnknownFieldSet unknownFields) {
        return this;
      }


      // @@protoc_insertion_point(builder_scope:lit_frame.config.hero_skill)
    }

    // @@protoc_insertion_point(class_scope:lit_frame.config.hero_skill)
    private static final lit_frame.config.ExcelPbDesc.hero_skill DEFAULT_INSTANCE;
    static {
      DEFAULT_INSTANCE = new lit_frame.config.ExcelPbDesc.hero_skill();
    }

    public static lit_frame.config.ExcelPbDesc.hero_skill getDefaultInstance() {
      return DEFAULT_INSTANCE;
    }

    private static final com.google.protobuf.Parser<hero_skill>
        PARSER = new com.google.protobuf.AbstractParser<hero_skill>() {
      public hero_skill parsePartialFrom(
          com.google.protobuf.CodedInputStream input,
          com.google.protobuf.ExtensionRegistryLite extensionRegistry)
          throws com.google.protobuf.InvalidProtocolBufferException {
          return new hero_skill(input, extensionRegistry);
      }
    };

    public static com.google.protobuf.Parser<hero_skill> parser() {
      return PARSER;
    }

    @java.lang.Override
    public com.google.protobuf.Parser<hero_skill> getParserForType() {
      return PARSER;
    }

    public lit_frame.config.ExcelPbDesc.hero_skill getDefaultInstanceForType() {
      return DEFAULT_INSTANCE;
    }

  }

  private static final com.google.protobuf.Descriptors.Descriptor
    internal_static_lit_frame_config_hero_skill_descriptor;
  private static final 
    com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
      internal_static_lit_frame_config_hero_skill_fieldAccessorTable;

  public static com.google.protobuf.Descriptors.FileDescriptor
      getDescriptor() {
    return descriptor;
  }
  private static  com.google.protobuf.Descriptors.FileDescriptor
      descriptor;
  static {
    java.lang.String[] descriptorData = {
      "\n\023excel_pb_desc.proto\022\020lit_frame.config\"" +
      "3\n\nhero_skill\022\017\n\007type_id\030\001 \001(\r\022\024\n\014displa" +
      "y_name\030\002 \001(\tb\006proto3"
    };
    com.google.protobuf.Descriptors.FileDescriptor.InternalDescriptorAssigner assigner =
        new com.google.protobuf.Descriptors.FileDescriptor.    InternalDescriptorAssigner() {
          public com.google.protobuf.ExtensionRegistry assignDescriptors(
              com.google.protobuf.Descriptors.FileDescriptor root) {
            descriptor = root;
            return null;
          }
        };
    com.google.protobuf.Descriptors.FileDescriptor
      .internalBuildGeneratedFileFrom(descriptorData,
        new com.google.protobuf.Descriptors.FileDescriptor[] {
        }, assigner);
    internal_static_lit_frame_config_hero_skill_descriptor =
      getDescriptor().getMessageTypes().get(0);
    internal_static_lit_frame_config_hero_skill_fieldAccessorTable = new
      com.google.protobuf.GeneratedMessageV3.FieldAccessorTable(
        internal_static_lit_frame_config_hero_skill_descriptor,
        new java.lang.String[] { "TypeId", "DisplayName", });
  }

  // @@protoc_insertion_point(outer_class_scope)
}