package com.pauloreis.catalog.admin.application.category.update;

import com.pauloreis.catalog.admin.application.UseCase;
import com.pauloreis.catalog.admin.domain.validation.handler.Notification;

import io.vavr.control.Either;

public abstract class UpdateCategoryUseCase
    extends UseCase<UpdateCategoryCommand, Either<Notification, UpdateCategoryOutput>> {

}
