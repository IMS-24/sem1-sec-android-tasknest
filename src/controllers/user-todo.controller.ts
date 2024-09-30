import { Body, Controller, Get, Param, Patch, Post } from '@nestjs/common';
import { UserTodoService } from '../services/user-todo.service';

@Controller('user-todo')
export class UserTodoController {
  constructor(private readonly userTodoService: UserTodoService) {}

  @Post('assign')
  async assignTodo(
    @Body() assignData: { userId: number; todoId: number; isOwner?: boolean },
  ) {
    return this.userTodoService.assignTodoToUser(
      assignData.userId,
      assignData.todoId,
      assignData.isOwner,
    );
  }

  @Patch(':userTodoId/status')
  async updateStatus(
    @Param('userTodoId') userTodoId: number,
    @Body() statusData: { status: string },
  ) {
    return this.userTodoService.updateStatus(userTodoId, statusData.status);
  }

  @Get('user/:userId')
  async findTodosByUser(@Param('userId') userId: number) {
    return this.userTodoService.findTodosByUser(userId);
  }
}
